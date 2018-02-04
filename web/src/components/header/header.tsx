import * as React from "react";
import { InputForm } from "./inputForm";

type HeaderProps = { };

export class Header extends React.Component<HeaderProps, {}> {
    render() {
        return (
           <InputForm/>
        );
    }
}