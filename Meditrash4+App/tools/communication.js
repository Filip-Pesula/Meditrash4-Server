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

    static createcategoryAdditionRequest() {

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
                .ele('category', item.category).up()
                .ele('weight', item.weight).up()
        }

        return root.end({ pretty: true });
    }

    static createItemsAcquiringRequest(token) {
        const object_structure = {
            Request: {
                uniqueToken: { '#text': token },
                requestCommand: { '@name': 'getItems' }
            }
        };

        return xmlbuilder.create(object_structure).end({ pretty: true });
    }

    static createDepartmentAcquiringRequest(token) {
        const object_structure = {
            Request: {
                uniqueToken: { '#text': token },
                requestCommand: { '@name': 'getDepartments' }
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

    static createFavItemRemovalRequest(token, items) {
        let root = xmlbuilder.create('Request');

        root.ele('uniqueToken', token);

        let requestCommand = root.ele('requestCommand', { name: 'removeFavItem' });

        for (let item of items) {
            requestCommand.ele('id', item.id)
        }

        return root.end({ pretty: true });
    }


    static createRecordRemovalRequest(token, records) {
        let root = xmlbuilder.create('Request');

        root.ele('uniqueToken', token);

        let requestCommand = root.ele('requestCommand', { name: 'removeRecord' });

        for (let record of records) {
            requestCommand.ele('id', record.id)
        }

        return root.end({ pretty: true });
    }

    static createPasswordEditRequest(token, password) {
        let root = xmlbuilder.create('Request');

        root.ele('uniqueToken', token);

        let requestCommand = root.ele('requestCommand', { name: 'editPassword' });

        requestCommand.ele('password', password)

        return root.end({ pretty: true });
    }

    static createFavListAcquiringRequest(token) {
        const object_structure = {
            Request: {
                uniqueToken: { '#text': token },
                requestCommand: { '@name': 'getFavList' }
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

    static createAddUserRequest(token, department , name, rodCislo,firstName,lastName,password, rights) {
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
                    lastName: { '#text': lastName },
                },
            }
        };

        return xmlbuilder.create(object_structure).end({ pretty: true });
    }

    static createThreshRecordsRequest(token, startDate, endDate, category) {
        let root = xmlbuilder.create('Request');

        root.ele('uniqueToken', token);

        let requestCommand = root.ele('requestCommand', { name: 'getTrashItem' });

        requestCommand.ele('catheory', category);
        requestCommand.ele('yearStart', startDate.getFullYear());
        requestCommand.ele('monthStart', startDate.getMonth() + 1);
        requestCommand.ele('dayStart', startDate.getDate());
        requestCommand.ele('yearEnd', endDate.getFullYear());
        requestCommand.ele('monthEnd', endDate.getMonth() + 1);
        requestCommand.ele('dayEnd', endDate.getDate());

        return root.end({ pretty: true });
    }

    static createCathegoriesListRequest(token) {
        let root = xmlbuilder.create('Request');

        root.ele('uniqueToken', token);
        root.ele('requestCommand', { name: 'getCathegories' });

        return root.end({ pretty: true });
    }

    static createRespPersonAdditionRequest() { }

    static createExportBycategoryRequest() { }
}

class ResponseParser {

    static async parseResponse(response) {
        const dataObject = await xml2js.parseStringPromise(response.toString());
        const rootElementName = Object.keys(dataObject)[0];

        if (rootElementName === 'Login') {
            return this.parseLoginResponse(response);
        }

        const responseType = dataObject[rootElementName].requestCommand[0].$.name;

        switch (responseType) {
            case 'getFavList': return this.parseFavListAcquiringResponse(response);
            case 'getItems': return this.parseItemsAcquiringResponse(response);
            case 'getDepartments': return this.parseDepartmentsAcquiringResponse(response);
            case 'getTrashItem': return this.parseThreshRecordsResponse(response);
            case 'getCathegories': return this.parseCathegoriesListResponse(response);
            case 'addUser': return this.parseAddUserResponse(response);
            case 'editPassword', 'trashItem': return true;

        }
    }

    static async parseCathegoriesListResponse(response) {
        const dataObject = await xml2js.parseStringPromise(response.toString());
        let categories = [];

        if (dataObject.Request.requestCommand[0].cathegory === undefined) return null;

        for (const category of dataObject.Request.requestCommand[0].cathegory) {
            categories.push(category.$);
        }

        return categories;
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

        if (dataObject.Request.requestCommand[0].item === undefined) return null;

        for (const item of dataObject.Request.requestCommand[0].item) {
            favItems.push(item.$);
        }

        return favItems;
    }

    static async parseThreshRecordsResponse(response) {
        const dataObject = await xml2js.parseStringPromise(response.toString());
        let records = [];

        if (dataObject.Request.requestCommand[0].record === undefined) return null;

        for (const record of dataObject.Request.requestCommand[0].record) {
            records.push(record.$);
        }

        return records;
    }

    static async parseAddUserResponse(response) {
        const dataObject = await xml2js.parseStringPromise(response.toString());
        if(Object.keys(dataObject).includes('RequestError')){
            throw new Error("");
        }
        return true;
    }

    static async parseItemsAcquiringResponse(response) {
        const dataObject = await xml2js.parseStringPromise(response.toString());
        let items = [];

        for (const item of dataObject.Request.requestCommand[0].item) {
            items.push(item.$);
        }

        return items;
    }

    static async parseDepartmentsAcquiringResponse(response) {
        const dataObject = await xml2js.parseStringPromise(response.toString());
        let departments = [];

        for (const item of dataObject.Request.requestCommand[0].department) {
            departments.push(item.$);
        }

        return departments;
    }
}



// <Login><name>ROOT</name><password>root</password></Login>


module.exports = { RequestAssembler, ResponseParser }