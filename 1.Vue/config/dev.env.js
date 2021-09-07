// 'use strict'
module.exports = {
  BASE_API: "https://localhost:44306/api/v1",
  FORM_ADD: 1,
  FORM_UPDATE: 2,
  FORM_CLONE: 3,
  FORM_SAVE_ADD: 4,
  STATUS : {
    NO_CONTENT: 204,
    BAD_REQUEST: 400,
    SERVER_ERROR: 500,
    NOT_FOUND: 400
  },
  MSG_TYPE: {
    SUCCESS: 'success',
    WARNING: 'warning',
    ERROR: 'error',
    INFO: 'info'
  }
}