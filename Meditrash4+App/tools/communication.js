const xmlbuilder = require('xmlbuilder')
const xml2js = require('xml2js')

class RequestAssembler {
    static removeHeader(data) {
        return data.replace('<?xml version="1.0"?>\n', '')
    }

    static createLoginRequest(name, password) {
        const object_structure = {
            Login: {
                name: { '#text': name },
                password: { '#text': password }
            }
        };

        return this.removeHeader(xmlbuilder
            .create(object_structure)
            .end({ pretty: true })
        );
    }

    static createUserAdditionRequest(token, department, name, password, rodCislo, rights, firstName, lastName) {
        const object_structure = {
            Request: {
                uniqueToken: { '#text': token },
                requestCommand: {
                    '@name': 'addUser',
                    department: { '#text': department },
                    name: { '#text': name },
                    password: { '#text': password },
                    rodCislo: { '#text': rodCislo },
                    rights: { '#text': rights },
                    firstName: { '#text': firstName },
                    lastName: { '#text': lastName }
                }
            }
        };

        return xmlbuilder.create(object_structure).end({ pretty: true });
    }

    static createDepartmentAdditionRequest(token, name) {
        const object_structure = {
            Request: {
                uniqueToken: { '#text': token },
                requestCommand: {
                    '@name': 'addDepartment',
                    name: { '#text': name },
                }
            }
        };

        return xmlbuilder.create(object_structure).end({ pretty: true });
    }

    static createCathegoryAdditionRequest() {

    }

    static createItemAdditionRequest(token, items) {
        let root = xmlbuilder.create('Request');

        root.ele('uniqueToken', token);

        let requestCommand = root.ele('requestCommand', { name: 'addItem' });

        for (let item of items) {
            requestCommand
                .ele('trash')
                .ele('id', item.id).up()
                .ele('name', item.name).up()
                .ele('cathegory', item.cathegory).up()
                .ele('weight', item.weight).up()
        }

        return root.end({ pretty: true });
    }

    static createItemsAcquiringRequest(token) {
        const object_structure = {
            Request: {
                uniqueToken: { '#text': token },
                requestCommand: {'@name': 'getItems'}
            }
        };
        
        return xmlbuilder.create(object_structure).end({ pretty: true });
    }

    static createFavItemAdditionRequest(token, items) {
        let root = xmlbuilder.create('Request');

        root.ele('uniqueToken', token);

        let requestCommand = root.ele('requestCommand', { name: 'addFavItem' });

        for (let item of items) {
            requestCommand.ele('id', item.id)
        }

        return root.end({ pretty: true });
    }

    static createFavListAcquiringRequest(token) {
        const object_structure = {
            Request: {
                uniqueToken: { '#text': token },
                requestCommand: {'@name': 'getFavList'}
            }
        };
        
        return xmlbuilder.create(object_structure).end({ pretty: true });
    }

    static createThreshItemRequest(token, id, count) {
        const object_structure = {
            Request: {
                uniqueToken: { '#text': token },
                requestCommand: {
                    '@name': 'trashItem',
                    id: { '#text': id },
                    count: { '#text': count }
                },
            }
        };

        return xmlbuilder.create(object_structure).end({ pretty: true });
    }

    static createRespPersonAdditionRequest() { }

    static createExportByCathegoryRequest() { }
}

class ResponseParser {

    static async parseResponse(response) {
        const dataObject = await xml2js.parseStringPromise(response.toString())
        const rootElementName = Object.keys(dataObject)[0];

        if (rootElementName === 'Login') {
            return this.parseLoginResponse(response);
        }

        console.log(dataObject[rootElementName]);
        const responseType = dataObject[rootElementName].RequestCommand[0].$.name;

        switch (responseType) {
            case 'getFavList': return this.parseFavListAcquiringResponse(response);
            case 'getItems': return this.parseItemsAcquiringResponse(response);
        }
    }

    static async parseLoginResponse(response) {
        const dataObject = await xml2js.parseStringPromise(response.toString())

        if (dataObject.Login.uniqueToken[0] == 0) {
            return { token: dataObject.Login.uniqueToken[0] };
        }

        return {
            token: dataObject.Login.uniqueToken[0],
            personalNumber: dataObject.Login.rodCislo[0],
            username: dataObject.Login.userName[0],
            firstname: dataObject.Login.firstName[0],
            lastname: dataObject.Login.lastName[0],
            department: dataObject.Login.department[0],
            rights: dataObject.Login.rights[0]
        };
    }

    static async parseFavListAcquiringResponse(response) {
        const dataObject = await xml2js.parseStringPromise(response.toString());
        let favItems = [];

        for (const item of dataObject.Request.RequestCommand[0].item) {
            favItems.push(item.$);
        }

        return favItems;
    }

    static async parseItemsAcquiringResponse(response) {
        const dataObject = await xml2js.parseStringPromise(response.toString());
        let items = [];

        for (const item of dataObject.Request.RequestCommand[0].item) {
            items.push(item.$);
        }
        
        return items;
    }
}



// <Login><name>ROOT</name><password>root</password></Login>


module.exports = { RequestAssembler, ResponseParser }