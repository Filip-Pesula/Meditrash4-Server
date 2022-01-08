const { contextBridge, ipcRenderer } = require('electron')

contextBridge.exposeInMainWorld('tools', {
    evaluateFormData: (data) => ipcRenderer.invoke('evaluate_login_data', data),
    continueToMainPage: () => ipcRenderer.invoke('continue_to_main_page'),
    getLoadedUserData: () => ipcRenderer.invoke('get_loaded_user_data')
})