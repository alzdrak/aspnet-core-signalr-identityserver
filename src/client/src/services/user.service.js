import request from "../utils/request";

export async function query() {
  return request("/api/users");
}

export async function queryCurrent() {
  return request("/api/currentUser");
}

// export async function removeRule(params) {
//   return request("/api/rule", {
//     method: "POST",
//     body: {
//       ...params,
//       method: "delete"
//     }
//   });
// }

// export async function addRule(params) {
//   return request("/api/rule", {
//     method: "POST",
//     body: {
//       ...params,
//       method: "post"
//     }
//   });
// }

// export async function updateRule(params) {
//   return request("/api/rule", {
//     method: "POST",
//     body: {
//       ...params,
//       method: "update"
//     }
//   });
// }

// export async function fakeSubmitForm(params) {
//   return request("/api/forms", {
//     method: "POST",
//     body: params
//   });
// }

// export async function queryFakeList(params) {
//   return request(`/api/fake_list?${stringify(params)}`);
// }

// export async function removeFakeList(params) {
//   const { count = 5, ...restParams } = params;
//   return request(`/api/fake_list?count=${count}`, {
//     method: "POST",
//     body: {
//       ...restParams,
//       method: "delete"
//     }
//   });
// }

// export async function fakeAccountLogin(params) {
//   return request("/api/login/account", {
//     method: "POST",
//     body: params
//   });
// }

// export async function fakeRegister(params) {
//   return request("/api/register", {
//     method: "POST",
//     body: params
//   });
// }

// export async function queryNotices() {
//   return request("/api/notices");
// }

// export async function getFakeCaptcha(mobile) {
//   return request(`/api/captcha?mobile=${mobile}`);
// }
