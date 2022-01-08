class InvalidCredentialsError extends Error {
    constructor(...params) {
      super(...params)
  
      if (Error.captureStackTrace) {
        Error.captureStackTrace(this, InvalidCredentialsError)
      }
  
      this.name = 'InvalidCredentialsError'
      this.date = new Date()
    }
}

class OperationNotPermitted extends Error {
  constructor(...params) {
    super(...params)

    if (Error.captureStackTrace) {
      Error.captureStackTrace(this, InvalidCredentialsError)
    }

    this.name = 'OperationNotPermitted'
    this.date = new Date()
  }
}

module.exports = { InvalidCredentialsError, OperationNotPermitted };