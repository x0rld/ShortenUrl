import React from "react";
export interface formAttributes {
    name: string,
    type: string,
    placeholder: string,
    value: any,
    min: number | undefined,
    handler: Function,
    required:boolean
}

export function Input(attributes: formAttributes) {
    const onChangeHandler = (e: React.ChangeEvent<HTMLInputElement>) => {
        attributes.handler(e.target.value)
    }
    if (attributes.type === "number")
        return <><label htmlFor={attributes.name}>{attributes.name}</label>
            <input name={attributes.name}
                   id={attributes.name}
                   type={attributes.type}
                   placeholder={attributes.placeholder}
                   min={attributes.min}
                   value={attributes.value}
                   required={attributes.required}
                   onChange={onChangeHandler}/></>;

    return <><label htmlFor={attributes.name}>{attributes.name}</label>
        <input name={attributes.name}
               id={attributes.name}
               type={attributes.type}
               required={attributes.required}
               placeholder={attributes.placeholder}
               onChange={onChangeHandler}/></>;
}