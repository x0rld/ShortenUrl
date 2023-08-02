
interface Payload {
    url :URL,
    size:number
}

export interface Response {
    errors: Errors
    success:boolean
    url:URL
}

export interface Errors {
    url: string[]
    size:string[]
}

export const createShortlink = async (payload:Payload):Promise<Response> => {
    const response = await fetch(`${API_URL}/api/shorten/`,
        {
            method: "POST",
            body: JSON.stringify(payload),
            headers: new Headers({'content-type': 'application/json'}),
        })
    if (response.ok) {
        const result = await response.json()
        result.success=true
        return result
    }
    else {
        const result = await response.json()
        result.success=false
        return result
    }
}
console.log(import.meta.env.VITE_API_URL);
const API_URL = import.meta.env.VITE_API_URL;