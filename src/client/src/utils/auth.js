import fetch from "node-fetch";

class Auth {
  constructor() {
    this.isAuthenticated = false;
    this.token = {};
  }

  // authenticate(cb) {
  //   this.isAuthenticated = true;
  //   setTimeout(cb, 100); // fake async
  // }

  signOut(cb) {
    this.isAuthenticated = false;
    setTimeout(cb, 100);
  }

  getIsAuthenticated() {
    return this.isAuthenticated;
  }

  setIsAuthenticated(value) {
    this.isAuthenticated = value;
  }

  /**
   * encode as uri string eg. ?client=test&username=alice...
   */
  encodeParams(params) {
    let body = "";
    for (let key in params) {
      if (body.length) {
        body += "&";
      }
      body += key + "=";
      body += encodeURIComponent(params[key]);
    }
    return body;
  }

  // login(username, password) {
  //   let tokenEndpoint: string = Config.TOKEN_ENDPOINT;

  //   let params: any = {
  //     client_id: Config.CLIENT_ID,
  //     grant_type: Config.GRANT_TYPE,
  //     username: username,
  //     password: password,
  //     scope: Config.SCOPE,
  //     site_id: siteId
  //   };

  //   let body: string = this.encodeParams(params);

  //   this.authTime = new Date().valueOf();

  //   return this.http
  //     .post(tokenEndpoint, body, this.options)
  //     .map((res: Response) => {
  //       let body: any = res.json();
  //       if (typeof body.access_token !== "undefined") {
  //         // Stores access token & refresh token.
  //         this.store(body);
  //         this.getUserInfo();

  //         // Tells all the subscribers about the new status.
  //         this.signinSubject.next(true);
  //       }
  //     })
  //     .catch((error: any) => {
  //       return Observable.throw(error);
  //     });
  // }

  authenticate(username, password) {
    var self = this;

    this.requestToken(username, password).then(function(data) {
      console.log(data);

      self.isAuthenticated = true;

      self.token = {
        access_token: data.access_token,
        expires_in: data.expires_in,
        refresh_token: data.refresh_token
      };
    });
  }

  requestToken(username, password) {
    let params = {
      client_id: process.env.REACT_APP_API_CLIENT_ID,
      client_secret: "secret",
      grant_type: "password",
      username: username,
      password: password,
      scope: process.env.REACT_APP_API_ENDPOINT_SCOPE
    };

    //http request
    return fetch(process.env.REACT_APP_TOKEN_ENDPOINT, {
      method: "POST",
      headers: {
        "Content-Type": "application/x-www-form-urlencoded"
      },
      body: this.encodeParams(params)
    })
      .then(function(response) {
        return response.json();
      })
      .catch(function(ex) {
        console.log("parsing failed", ex);
      });
  }

  // //get new token from identity server
  // requestToken(username, password) {
  //   let tokenEndpoint = process.env.API_URL;

  //   var self = this;

  //   let params = {
  //     client_id: process.env.API_CLIENT_ID,
  //     client_secret: "secret",
  //     grant_type: "password",
  //     username: username,
  //     password: password,
  //     scope: process.env.API_ENDPOINT_SCOPE
  //   };

  //   //http request
  //   return fetch("http://localhost:50000/connect/token", {
  //     method: "POST",
  //     headers: {
  //       "Content-Type": "application/x-www-form-urlencoded"
  //     },
  //     body: this.encodeParams(params)
  //   })
  //     .then(function(response) {
  //       return response.json();
  //     })
  //     .then(function(data) {
  //       if (typeof data.access_token !== "undefined") {
  //         self.isAuthenticated = true;

  //         self.token = {
  //           access_token: data.access_token,
  //           expires_in: data.expires_in,
  //           refresh_token: data.refresh_token
  //         };

  //         // Stores access token & refresh token.
  //         //this.store(body);
  //         //this.getUserInfo();
  //         // Tells all the subscribers about the new status.
  //         //this.signinSubject.next(true);
  //       }
  //     })
  //     .catch(function(ex) {
  //       console.log("parsing failed", ex); // eslint-disable-line no-console
  //     });
  // }

  getToken() {
    return this.token;
  }
}

//singleton
const auth = new Auth();
export default auth;
