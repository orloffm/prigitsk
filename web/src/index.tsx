import * as React from "react";
import * as ReactDOM from "react-dom";

import 'bootstrap/dist/css/bootstrap.css';
import './my.css';
import { Hello } from "./components/hello";

ReactDOM.render(
    <Hello compiler="TypeScript" framework="React" />,
    document.getElementById("example")
);