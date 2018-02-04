import * as React from "react";
import * as styles from '../prigitsk.css';
import { Header } from "./header";
import { Viewport } from "./viewport";
import { Options } from "./options";
import { Footer } from "./footer";

export interface PrigitskAppProps { }

// 'HelloProps' describes the shape of props.
// State is never set so we use the '{}' type.
export class PrigitskApp extends React.Component<PrigitskAppProps, {}> {
    render() {
        return (
            <div className={styles.prigitskContainer}>
                <div className={styles.prigitskHeader}>
                    <Header/>
                </div>  
                <div className={styles.prigitskViewport}>
                   <Viewport/>
                </div>  
                <div className={styles.prigitskOptions}>
                   <Options/>
                </div>  
                <div className={styles.prigitskFooter}>
                   <Footer/>
                </div>
            </div>
        );
    }
}