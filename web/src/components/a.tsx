import * as React from "react";

export interface AProps { }

export class A extends React.Component<AProps, {}> {
    render() {
        return <span>A here!</span>;
    }
}