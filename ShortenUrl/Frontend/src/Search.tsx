import {Input} from "./Input.tsx";
import {getShortlink} from "./ApiCall.ts";
import  {useState} from "react";

export function Search() {
    const [token,setToken] = useState<string>("")
    const [link, setLink] = useState<URL>()
    const [error,setError] = useState<Error>()
    function submit() {
        getShortlink(token).then(response => {
            setError(undefined)
            if (response.success){
                setLink(response.url)
                return
            }
            setError(response.error)
        })
        setLink(undefined);
    }

    return<>
        <Input name="token" type="text" min={undefined} placeholder="token size" value={null}
               handler={setToken} required={true}/>
        <button onClick={submit}>get short url!</button>
        <br/>
        <a href={link?.toString()}>{link?.toString()}</a>
        <p style={{color: 'red'}}>
            {error?.message}
        </p>
    </>
}