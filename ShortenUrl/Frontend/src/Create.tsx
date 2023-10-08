import React, {useState} from "react";
import {createShortlink} from "./ApiCall.ts";
import {Input} from "./Input.tsx";


export function Create() {
    const [url, setUrl] = useState<URL>()
    const [size, setSize] = useState(5)

    const [link, setLink] = useState<URL>()
    const [errors, setErrors] = useState<Array<any>>([])
    const submit = (e: React.MouseEvent) => {
        e.preventDefault();
        const errorMessages = []
        if (size < 5 || size > 20) {
            errorMessages.push("the minimum size is 5 and maximum is 20")
        }
        if (url == null || url.toString() === "") {
            errorMessages.push("invalid input")
        }

        if (errorMessages.length) {
            setErrors(errorMessages)
            return;
        }
        if (url)
            createShortlink({url, size}).then(response => {
                if (!response.success) {
                    setErrors([...response.errors.url, response.errors.size].filter(it => it != undefined))
                    return
                }
                setErrors([])
                setLink(response.url)
            })
    }
    return <>
        <div className="flex justify-center ">
            <div className="flex-col m-5">
                <div className="card w-96 bg-neutral text-neutral-content">
                    <div className="card-body items-center text-center">
                        <form>
                            <Input name={"url"} type={"text"} placeholder={"https://google.com"} key={"url"} min={0}
                                   value={url}
                                   handler={setUrl} required={true}/>
                            <Input name="size" type="number" min={10} placeholder="token size" value={size.toString()}
                                   handler={setSize} required={true}/>
                            <button onClick={submit} className="btn m-1">short url!</button>
                        </form>
                    </div>
                </div>
                <div className="text-red-600 text-center m-2">
                    <ul>
                        {
                            errors.map((it: string) => <li key={it}>{it}</li>)
                        }
                    </ul>
                </div>
                <div className="text-center underline hover:cursor-auto">
                    <a href={link?.toString()}>{link?.toString()}</a>
                </div>
            </div>
        </div>
    </>;
}