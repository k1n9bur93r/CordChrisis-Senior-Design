// JavaScript source code
import React, { Component } from 'react';


export class LogIn extends Component{
    static displayName = LogIn.name;

    constructor() {
        super();
        this.state = {
            username: '',
            password: '',
            error: '',
        };

        this.handlePassChange = this.handlePassChange.bind(this);
        this.handleUserChange = this.handleUserChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.dismissError = this.dismissError.bind(this);

    }

    handleUserChange(event) {
        this.setState({
            username: event.target.value,
        })
    }

    handlePassChange(event) {
        this.setState({
            password: event.target.value,
        })
    }

    dismissError() {
        this.setState({ error: '' });
    }

    handleSubmit(event) {
        event.preventDefault();

        if (!this.state.username) {
            return this.setState({ error: 'Username is required' });
        }

        if (!this.state.password) {
            return this.setState({ error: 'Password is required' });
        }

        return this.setState({ error: '' });

    }



    render() {
        return (
            
            <div className="Login">
                <form onSubmit={this.handleSubmit}>{
                    this.state.error &&
                    <h3 data-test="error" onClick={this.dismissError}>
                        <button onClick={this.dismissError}>X</button>
                        {this.state.error}
                    </h3>
                }
                    <label>User Name</label>
                    <input type="text" data-test="username"
                        value={this.state.username}
                        onChange={this.handleUserChange} />

                    <label>Password </label>
                    <input type="password" data-test="password"
                        value={this.state.password}
                        onChange={this.handlePassChange} />

                    <input type="submit" vlaue="Log In" data-test="submit" />
                </form>

            </div>

            //<div className="Example">
            //    <div>
            //        <button onClick={() => this.imAfunction()}>PRESS ME</button>
            //    </div>
            //</div>
        );
    }
}




