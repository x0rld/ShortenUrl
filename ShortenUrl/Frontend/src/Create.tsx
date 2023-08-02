import React, {useState} from "react";
import {createShortlink} from "./ApiCall.ts";

interface formAttributes {
    name: string,
    type: string,
    placeholder: string,
    value: any,
    min: number | undefined,
    handler: Function
}

function Input(attributes: formAttributes) {
    const onChangeHandler = (e: React.ChangeEvent<HTMLInputElement>) => {
        attributes.handler(e.target.value)
    }
    if (attributes.type === "number")
        return <><label htmlFor={attributes.name}>{attributes.name}</label>
            <input name={attributes.name}
                   id={attributes.name}
                   type={attributes.type}
                   placeholder={attributes.placeholder}
                   min={attributes.min} value={attributes.value} onChange={onChangeHandler}/></>;

    return <><label htmlFor={attributes.name}>{attributes.name}</label>
        <input name={attributes.name}
               type={attributes.type}
               placeholder={attributes.placeholder} onChange={onChangeHandler}/></>;
}

export function Create() {
    const [url, setUrl] = useState<URL>()
    const [size, setSize] = useState(10)

    const [link, setLink] = useState<URL>()
    const [errors, setErrors] = useState<Array<any>>([])
    const submit = (e: React.MouseEvent) => {
        errors.length = 0
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
        createShortlink({url, size}).then(data => {
            errors.length = 0
            if (!data.success) {
                setErrors([...data.errors.url, data.errors.size].filter(it => it != undefined))
                return
            }
            setLink(data.url)
        })
    }
    return <>
        <div style={{color: 'red'}}>
            <ul>
                {
                    errors.map((it: string) => <li key={it}>{it}</li>)
                }
            </ul>
        </div>
        <form>
            <Input name={"url"} type={"text"} placeholder={"https://google.com"} key={"url"} min={0} value={url}
                   handler={setUrl}/>
            <Input name="size" type="number" min={10} placeholder="token size" value={size.toString()}
                   handler={setSize}/>
            <button onClick={submit}>short url!</button>
        </form>
        <a href={link?.toString()}>{link?.toString()}</a>
    </>;
}