class SharedData {
    #sharedDataObj;
    #ipcInstance;

    constructor(sharedDataObj, ipcInstance) {
        this.#sharedDataObj = sharedDataObj;
        this.#ipcInstance = ipcInstance;
    }

    getSharedDataObj() {
        return this.#sharedDataObj;
    }

    createHandlers() {

    }

    createHandles() {
        this.#ipcInstance.handle('get_loaded_user_data', async (event, arg) => {
            return new Promise((resolve, reject) => {
                try {
                    resolve({
                        firstName: this.#sharedDataObj.user.firstName,
                        lastName: this.#sharedDataObj.user.lastName,
                        rights: this.#sharedDataObj.user.rights,
                    });
                } catch (error) {
                    reject(error);
                }
            })
        })
    }
}

module.exports = SharedData;