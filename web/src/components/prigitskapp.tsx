import * as React from "react";
import * as styles from '../prigitsk.css';

export interface PrigitskAppProps { }

// 'HelloProps' describes the shape of props.
// State is never set so we use the '{}' type.
export class PrigitskApp extends React.Component<PrigitskAppProps, {}> {
    render() {
        return (
            <div>
                <div className={styles.prigitskHeader}>
                   
                </div>
            </div>
        );
    }
}