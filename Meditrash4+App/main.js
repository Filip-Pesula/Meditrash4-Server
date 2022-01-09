const RequestsHandler = require('./tools/server_requests_handlers.js')
const Router = require('./tools/router.js')
const { ipcMain } = require('electron');
const SharedData = require('./tools/shared_data.js');

// static variables, settings
const port = 16246;
const host = 'localhost';

// static variables, tools
const dataStore = new SharedData({}, ipcMain);
const requestsHandler = new RequestsHandler(ipcMain, host, port, dataStore);
const router = new Router(ipcMain, 'preload.js', 'login.html', dataStore);

// init app and add handlers for site routing
router.initApp().then(router.createHandles())

// add event handlers
requestsHandler.createHandles();
dataStore.createHandles();
