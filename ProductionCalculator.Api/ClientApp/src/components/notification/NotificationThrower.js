﻿import Store from "../../data/DataStore";
import {NotificationAddAction} from "../../data/reducers/NotificationReducer";

export const throwInfoNotification = function (message) {
  throwNotification(message, "info")
}

export const throwSuccessNotification = function (message) {
  throwNotification(message, "success")
}

export const throwWarningNotification = function (message) {
  throwNotification(message, "warning")
}

export const throwErrorNotification = function (message) {
  throwNotification(message, "error")
}

const throwNotification = function (message, type) {
  Store.dispatch(NotificationAddAction({message, type}));
}
