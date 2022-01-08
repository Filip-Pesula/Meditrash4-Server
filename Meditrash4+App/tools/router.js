const { app, BrowserWindow } = require('electron')
const { OperationNotPermitted } = require('./errors')
const path = require('path');

class Router {
    #ipcInstance;
    #window;
    #initPreload;
    #initView;
    #dataStore;

    constructor(ipcInstance, initPreload, initView, dataStore) {
        this.#ipcInstance = ipcInstance;
        this.#dataStore = dataStore;
        this.#initPreload = initPreload;
        this.#initView = initView;
    }

    static createWindow(preloadFileName, viewFileName, width = 1000, height = 800) {
        const win = new BrowserWindow({
            width,
            height,
            webPreferences: {
                preload: path.join(__dirname, '../preloads', preloadFileName)
            }
        });

        win.loadFile(path.join(__dirname, '../views', viewFileName));

        return win;
    }

    async initApp() {
        return new Promise((resolve, reject) => {

            try {
                app.whenReady().then(() => {
                    this.#window = Router.createWindow(this.#initPreload, this.#initView);

                    app.on('activate', () => {
                        if (BrowserWindow.getAllWindows().length === 0) {
                            this.#window = Router.createWindow(this.#initPreload, this.#initView);
                        }
                    })

                    resolve(true);
                })

                app.on('window-all-closed', () => {
                    if (process.platform !== 'darwin') {
                        app.quit()
                    }
                })
            } catch (error) {
                reject(error);
            }
        })
    }

    createHandles() {
        this.#ipcInstance.handle('continue_to_main_page', async (event, arg) => {
            return new Promise((resolve, reject) => {
                if (this.#dataStore.getSharedDataObj().user.token != 0) {
                    this.#window.loadFile(path.join(__dirname, '../views', 'in.html'));
                    resolve(true);
                }
                reject(new OperationNotPermitted('User\'s rights level is insufficient'));
            })
        })
    }
}

module.exports = Router;