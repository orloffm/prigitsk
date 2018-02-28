import * as React from "react";
import * as bs from 'bootstrap/dist/css/bootstrap.css';

type InputFormProps = { };
type InputFormState = { 
    repositoryUrl : string
};

export class InputForm extends React.Component<InputFormProps, InputFormState> {
   
    constructor(props: InputFormProps) {
        super(props);
        this.state = {repositoryUrl: ''};
    
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
      }

    handleChange(event: any) : void {
        this.setState({repositoryUrl: event.target.value});
    }
    
    handleSubmit(event: any) : void {
      alert('A name was submitted: ' + this.state.repositoryUrl);
      event.preventDefault();
    }
      
    render() {
        return (
            // <form onSubmit={this.handleSubmit.bind(this)}>
            //     <label>repository:</label>
            //     <input type="text"
            //         className="form-control"
            //         id="repositoryUrl"
            //         placeholder="url"
            //         value={this.state.repositoryUrl}
            //         onChange={this.handleChange.bind(this)}
            //         />
            <form
                className={bs.formInline}
                onSubmit={this.handleSubmit}>
                <label>repository:</label>
                <input
                    type="text"
                    value={this.state.repositoryUrl} 
                    onChange={this.handleChange}
                    className={bs.formControl}
                    id="repositoryUrl"
                    placeholder="url"/>
                <button
                    type="submit"
                    className={[bs.btn, bs.btnPrimary].join(' ')}>load</button>
            </form>
        );
    }
}