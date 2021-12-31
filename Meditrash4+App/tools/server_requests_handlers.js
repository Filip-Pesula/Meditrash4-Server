const { ipcMain } = require('electron')
const { RequestAssembler, ResponseParser } = require('./communication.js')
const net = require('net');

module.exports = class RequestsHandler {
    #ipcInstance;
    #host;
    #port;
    #dataStore;

    constructor(ipcInstance, host, port, dataStore) {
        this.#ipcInstance = ipcInstance;
        this.#host = host;
        this.#port = port;
        this.#dataStore = dataStore;
    }

    async prepareCommunication() {
        const client = net.createConnection({ port: this.#port, host: this.#host });

        client.on('data', async (response) => {
            try {
                // will be replaced by universal funtion for various request types
                const data = await ResponseParser.parseLoginResponse(response);
                client.emit('data_prepared', data);
            } catch (e) {
                client.emit('data_preparation_failure', e);
            }
        });

        return new Promise((resolve, reject) => {
            client.on('ready', () => resolve(client));
            client.on('error', (error) => reject(error));
        });
    }

    async writeData(data) {
        const client = await this.prepareCommunication();
        client.write(data, 'utf8');

        return new Promise((resolve, reject) => {
            client.on('data_prepared', (data) => resolve(data));
            client.on('data_preparation_failure', (exception) => reject(exception));
            client.on('end', () => reject('Connection closed without data acquired'))
        })
    }

    createHandles() {
        this.#ipcInstance.handle('evaluate_login_data', async (event, arg) => {
            const request = RequestAssembler.createLoginRequest(arg.name, arg.password);
            this.#dataStore.user = await this.writeData(request);
            return null;
        });

        this.#ipcInstance.handle('add_user', async (event, arg) => {
            data = RequestAssembler.createUserAdditionRequest(
                arg.token, arg.department, arg.name, arg.password,
                arg.rodCislo, arg.rights, arg.firstName, arg.lastName
            );

            return this.writeData(data);
        });

        this.#ipcInstance.handle('add_department', async (event, arg) => {
            data = RequestAssembler.createDepartmentAdditionRequest(arg.token, arg.name);
            return this.writeData(data);
        });

        this.#ipcInstance.handle('add_cathegory', async (event, arg) => {
            data = RequestAssembler.createCathegoryAdditionRequest();
            return this.writeData(data);
        });

        this.#ipcInstance.handle('add_item', async (event, arg) => {
            data = RequestAssembler.createItemAdditionRequest(arg.token, arg.items);
            return this.writeData(data);
        });

        this.#ipcInstance.handle('get_items', async (event, arg) => {
            data = RequestAssembler.createItemsAcquiringRequest(arg.token);
            return this.writeData(data);
        });

        this.#ipcInstance.handle('add_fav_item', async (event, arg) => {
            data = RequestAssembler.createFavItemAdditionRequest(arg.token, arg.items);
            return this.writeData(data);
        });

        this.#ipcInstance.handle('get_fav_list', async (event, arg) => {
            data = RequestAssembler.createFavListAcquiringRequest(arg.token);
            return this.writeData(data);
        });

        this.#ipcInstance.handle('thrash_item', async (event, arg) => {
            data = RequestAssembler.createThreshItemRequest();
            return this.writeData(data);
        });

        this.#ipcInstance.handle('add_resp_person', async (event, arg) => {
            data = RequestAssembler.createRespPersonAdditionRequest();
            return this.writeData(data);
        });

        this.#ipcInstance.handle('export_thrash_by_cathegory', async (event, arg) => {
            data = RequestAssembler.createExportByCathegoryRequest();
            return this.writeData(data);
        });
    }
}

