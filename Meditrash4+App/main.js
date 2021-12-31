const RequestsHandler = require('./tools/server_requests_handlers.js')
const { app, BrowserWindow } = require('electron')
const path = require('path')
const { ipcMain } = require('electron');

// static variables, settings
const port = 16246;
const host = 'localhost';

// static variables, tools
const dataStore = {};
const requestsHandler = new RequestsHandler(ipcMain, host, port, dataStore);
let window;

// app code
requestsHandler.createHandles();

ipcMain.handle('continue_to_main_page', (event, arg) => {
    if (dataStore.user.token != 0) {
        window.loadFile(path.join(__dirname, 'views', 'in.html'));
        return true;
    }
    throw 'Invalid username or password';
})

app.whenReady().then(() => {
    window = createWindow('login_preload.js', 'login.html');

    app.on('activate', () => {
        if (BrowserWindow.getAllWindows().length === 0) {
            window = createWindow('login_preload.js', 'login.html');
        }
    })
})

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') {
        app.quit()
    }
})

function createWindow(preloadFileName, viewFileName) {
    const win = new BrowserWindow({
        width: 1000,
        height: 800,
        webPreferences: {
            preload: path.join(__dirname, 'preloads', preloadFileName)
        }
    });

    win.loadFile(path.join(__dirname, 'views', viewFileName));

    return win;
}

