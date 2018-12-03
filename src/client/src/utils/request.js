//import React from "react";
//import fetch from 'dva/fetch';
//import { notification } from 'antd';
//import Snackbar from "@material-ui/core/Snackbar";
//import router from 'umi/router';
import { history } from "./browserRouter";
import hash from "hash.js";

// import { BrowserRouter } from 'react-router-dom';
// import { createBrowserHistory } from 'history';

// export const history = createBrowserHistory();

// export default class YourBrowserRouter extends BrowserRouter {
//   history;
// }

const codeMessage = {
  200: "The server successfully returned the requested data.", //200 OK - Standard response for successful HTTP requests.
  201: "New or modified data is successful.", //201 Created - The request has been fulfilled, resulting in the creation of a new resource
  202: "A request has entered the background queue (asynchronous task).", //202 Accepted
  204: "Deleted data successfully. ", //204 No Content - The server successfully processed the request and is not returning any content.
  400: "The request was made with an error, and the server did not perform operations to create or modify data. ", //400 Bad Request
  401: "The user is unauthorized (username, password is incorrect). ", //401 Unauthorized
  403: "User is authorized, but do not have permissions to access the request. ", //403 Forbidden
  404: "The request does not exist.", //404 Not Found
  406: "The format of the request is not available. ", //406 Not Acceptable - The requested resource is capable of generating only content not acceptable according to the Accept headers sent in the request
  410: "The requested resource was permanently deleted and is no longer available. ", //410 Gone
  422: "A validation error occurred while creating an object. ",
  500: "Internal Server Error.",
  502: "Gateway error.",
  503: "The service is currently unavailable, the server is temporarily overloaded or down for maintenance. ",
  504: "The gateway timed out. "
};

const checkStatus = response => {
  if (response.status >= 200 && response.status < 300) {
    return response;
  }

  const errortext = codeMessage[response.status] || response.statusText;
  // Snackbar.error({
  //   Message: ` Request error ${response.status} : ${response.url} `,
  //   description: errortext
  // });

  // var snackbar = React.createElement(Snackbar, {
  //   anchorOrigin: {vertical: "bottom", horizontal:"right"},
  //   open: true,
  //   ContentProps={
  //     "aria-describedby": "message-id"
  //   },
  //   message=(<span id="message-id">I love snacks</span>)
  // });

  // notification.error({
  //   Message: ` Request error ${response.status} : ${response.url} `,
  //   description: errortext
  // });
  const error = new Error(errortext);
  error.name = response.status;
  error.response = response;
  throw error;
};

const cachedSave = (response, hashcode) => {
  /**
   * Clone a response data and store it in sessionStorage
   * Does not support data other than json, Cache only json
   */
  const contentType = response.headers.get("Content-Type");
  if (contentType && contentType.match(/application\/json/i)) {
    // All data is saved as text
    response
      .clone()
      .text()
      .then(content => {
        sessionStorage.setItem(hashcode, content);
        sessionStorage.setItem(`${hashcode}:timestamp`, Date.now());
      });
  }
  return response;
};

/**
 * Requests a URL, returning a promise.
 *
 * @param  {string} url       The URL we want to request
 * @param  {object} [option] The options we want to pass to "fetch"
 * @return {object}           An object containing either "data" or "err"
 */
export default function request(url, option) {
  const options = {
    expires: true,
    ...option
  };
  /**
   * Produce fingerprints based on url and parameters
   * Maybe url has the same parameters
   */
  const fingerprint = url + (options.body ? JSON.stringify(options.body) : "");
  const hashcode = hash
    .sha256()
    .update(fingerprint)
    .digest("hex");

  const defaultOptions = {
    credentials: "include"
  };
  const newOptions = { ...defaultOptions, ...options };
  if (
    newOptions.method === "POST" ||
    newOptions.method === "PUT" ||
    newOptions.method === "DELETE"
  ) {
    if (!(newOptions.body instanceof FormData)) {
      newOptions.headers = {
        Accept: "application/json",
        "Content-Type": "application/json; charset=utf-8",
        ...newOptions.headers
      };
      newOptions.body = JSON.stringify(newOptions.body);
    } else {
      // newOptions.body is FormData
      newOptions.headers = {
        Accept: "application/json",
        ...newOptions.headers
      };
    }
  }

  const expires = options.expires && 60;
  // options.expires !== false, return the cache,
  if (options.expires !== false) {
    const cached = sessionStorage.getItem(hashcode);
    const whenCached = sessionStorage.getItem(`${hashcode}:timestamp`);
    if (cached !== null && whenCached !== null) {
      const age = (Date.now() - whenCached) / 1000;
      if (age < expires) {
        const response = new Response(new Blob([cached]));
        return response.json();
      }
      sessionStorage.removeItem(hashcode);
      sessionStorage.removeItem(`${hashcode}:timestamp`);
    }
  }

  //return result
  return fetch(url, newOptions)
    .then(checkStatus)
    .then(response => cachedSave(response, hashcode))
    .then(response => {
      // DELETE and 204 do not return data by default
      // using .json will report an error.
      if (newOptions.method === "DELETE" || response.status === 204) {
        return response.text();
      }
      return response.json();
    })
    .catch(e => {
      const status = e.name;
      if (status === 401) {
        // @HACK
        /* eslint-disable no-underscore-dangle */
        window.g_app._store.dispatch({
          type: "login/logout"
        });
        return;
      }
      // environment should not be used
      if (status === 403) {
        history.push("/exception/403");
        return;
      }
      if (status <= 504 && status >= 500) {
        history.push("/exception/500");
        return;
      }
      if (status >= 404 && status < 422) {
        history.push("/exception/404");
      }
    });
}
