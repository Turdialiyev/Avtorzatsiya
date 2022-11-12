export class  Result {
    constructor(userToken: string, error: Error)
    {
        this.data = userToken
        this.error = error
    }

    public data?: string;
    public error?: Error;
    ;
    
 
}
export class  Error {
    constructor(message: string, code: number)
    {
        this.message = message,
        this.code = code;
    }

    public message?: string;
    public code?: number;
}
