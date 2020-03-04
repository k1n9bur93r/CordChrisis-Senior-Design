import React, { Component } from 'react';
import './SearchSample.css';

export class SearchSample extends Component{
    static displayName = SearchSample.name;

    render() {
        return (
            <div>
                <h2> search sample page </h2>
                <table>
                    <tr>
                        <td>
                            <label> Song name&nbsp; </label>
                            <input type='text' value='' />
                        </td>
                        <td>
                            <label> Genre&nbsp; </label>
                            <input type='text' value=''/>
                        </td>
                        <td>
                            <label> Difficulty&nbsp; </label>
                            <select>
                                <option> Easy </option>
                                <option> Medium </option>
                                <option> Hard </option>
                            </select>
                        </td>
                    </tr>

                </table>
                <br/>
                <table class="result">
                    <tr>
                        <td class="result" rowspan="5">
                            image goes here
                        </td>
                    </tr>
                    <tr>
                        <td class="result">
                            Song name:
                        </td>    
                    </tr>
                    <tr>
                        <td class="result">
                            Genre:
                        </td>    
                    </tr>
                    <tr>
                        <td class="result">
                            High score:
                        </td>    
                    </tr>
                </table>
                <br />
                <table class="result">
                    <tr>
                        <td class="result" rowspan="5">
                            image goes here
                        </td>
                    </tr>
                    <tr>
                        <td class="result">
                            Song name:
                        </td>
                    </tr>
                    <tr>
                        <td class="result">
                            Genre:
                        </td>
                    </tr>
                    <tr>
                        <td class="result">
                            High score:
                        </td>
                    </tr>
                </table>
                <br />
                <table class="result">
                    <tr>
                        <td class="result" rowspan="5">
                            image goes here
                        </td>
                    </tr>
                    <tr>
                        <td class="result">
                            Song name:
                        </td>
                    </tr>
                    <tr>
                        <td class="result">
                            Genre:
                        </td>
                    </tr>
                    <tr>
                        <td class="result">
                            High score:
                        </td>
                    </tr>
                </table>
            </div>
        );
    }
}