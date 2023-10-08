
export interface urlInfo {
    url: URL,
    size: number
}

export interface CreateResponse {
    errors: ErrorCreateResponse
    success: boolean
    url: URL
}

export interface ErrorCreateResponse {
    url: string[]
    size: string[]
}

export const createShortlink = async (payload: urlInfo): Promise<CreateResponse> => {
    const response = await fetch(`${API_URL}/api/shorten/`,
        {
            method: "POST",
            body: JSON.stringify(payload),
            headers: new Headers({'content-type': 'application/json',
            }),
        })
    if (response.ok) {
        const result = await response.json()
        result.success = true
        return result
    } else {
        const result = await response.json()
        result.success = false
        return result
    }
}

export const getShortlink = async (payload: string): Promise<QueryResponse> => {
    if (payload == "") {
        return {success: false, error: {message: "token not valid"} as Error} as QueryResponse
    }
    let response = undefined;
    try {
        response = await fetch(`${API_URL}/${payload}`)
    }
    catch (err) {
        // We have to verify err is an
        // error before using it as one.
        if (err instanceof Error && err.message === "Failed to fetch") {
            return {url: new URL(`${API_URL}/${payload}`), success: true} as QueryResponse
        }
        return {success: false, error: {message: "error try later"} as Error} as QueryResponse
    }
    if (!response.ok){
        return {success: false, error: {message: "error try later"} as Error} as QueryResponse
    }
    
    if (response?.status === 404) {
        return {success: false, error: {message: "token not found"} as Error} as QueryResponse
    }
    return {success: false, error: {message: "error try later"} as Error} as QueryResponse
}

export interface QueryResponse {
    error: Error | undefined
    success: boolean
    url: URL
}

const API_URL = "https://localhost:7056";