import * as React from "react";

export interface ViewportProps { }

export class Viewport extends React.Component<ViewportProps, {}> {
    render() {
        return (
            <div>
                I am viewport.
            </div>
        );
    }
}