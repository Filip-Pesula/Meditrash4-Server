const { app, BrowserWindow } = require('electron')
const path = require('path')

class Components {
    static createLoginWindow() {
        function createWindow() {
            const win = new BrowserWindow({
                width: 1000,
                height: 800,
                webPreferences: {
                    preload: path.join(__dirname, 'preloads', 'login_preload.js')
                }
            });

            win.loadFile('./views/login.html');

            return win;
        }

        return new Promise((resolve, reject) => {
            try {
                app.whenReady().then(() => {
                    createWindow()

                    app.on('activate', () => {
                        if (BrowserWindow.getAllWindows().length === 0) {
                            resolve(createWindow());
                        }
                    })
                })

                app.on('window-all-closed', () => {
                    if (process.platform !== 'darwin') {
                        app.quit()
                    }
                })
            } catch (e) {
                reject(e);
            }
        });
    }
}

module.exports = Components