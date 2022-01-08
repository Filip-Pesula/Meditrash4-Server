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

    createHandles() {
        this.#ipcInstance.handle('get_loaded_user_data', async (event, arg) => {
            return new Promise((resolve, reject) => {
                try {
                    resolve({
                        personalNumber: this.#sharedDataObj.user.personalNumber,
                        username: this.#sharedDataObj.user.username,
                        firstname: this.#sharedDataObj.user.firstname,
                        lastname: this.#sharedDataObj.user.lastname,
                        department: this.#sharedDataObj.user.department,
                        rights: this.#sharedDataObj.user.rights,
                        favItems: this.#sharedDataObj.user.favItems
                    });
                } catch (error) {
                    reject(error);
                }
            })
        })
    }
}

module.exports = SharedData;