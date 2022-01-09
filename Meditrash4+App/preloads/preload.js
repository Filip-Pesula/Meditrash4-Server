const { contextBridge, ipcRenderer, ipcMain } = require('electron')

contextBridge.exposeInMainWorld('serverRequests', {
    evaluateFormData: (data) => ipcRenderer.invoke('evaluate_login_data', data),
    getUserFavItems: () => ipcRenderer.invoke('get_fav_list'),
    getAllItems: () => ipcRenderer.invoke('get_items'),
    addNewThrashRecord: (data) => ipcRenderer.invoke('thrash_item', data),
    removeFavItems: (data) => ipcRenderer.invoke('remove_fav_item', data),
    addNewFavItems: (data) => ipcRenderer.invoke('add_fav_item', data),
    editPassword: (data) => ipcRenderer.invoke('edit_password', data)
})

contextBridge.exposeInMainWorld('localData', {
    getLoadedUserData: () => ipcRenderer.invoke('get_loaded_user_data'), 
})

contextBridge.exposeInMainWorld('router', {
    goToStatisticsPage: () => ipcRenderer.invoke('go_to_statistics_page'),
    goToRecordViewPage: () => ipcRenderer.invoke('go_to_record_view_page'),
    goToProfilePage: () => ipcRenderer.invoke('go_to_profile_page'),
    goToAddRecordPage: () => ipcRenderer.invoke('go_to_add_record_page'),
    continueToMainPage: () => ipcRenderer.invoke('continue_to_main_page'),
    exitToLogin: () => ipcRenderer.invoke('exit_to_login')
})