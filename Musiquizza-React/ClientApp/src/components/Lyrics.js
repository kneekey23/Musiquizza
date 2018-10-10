import React, { Component } from 'react';
import { Button } from 'react-bootstrap';
import { API_ROOT } from './api-config';


export class Lyrics extends Component {
    constructor(props) {
        super(props);
        this.state = { lyrics: "" };
        this.getLyrics = this.getLyrics.bind(this);
    }

    componentDidMount() {
        this.getLyrics();
    }
    getLyrics() {
        fetch(`${API_ROOT}/Lyrics/GetLyric`, {
            headers: new Headers({
                "Accept": "application/json"
            })
        })
            .then(response => response.json())
            .then(json => this.setState({ lyrics: json.lyrics }))
            .catch(error => console.log(error))
        }


    render() {

        return (
             
                    <div className="row">
                        <div className="col-md-10">
                            <b>{this.state.lyrics}</b>
                        </div>
                        <div className="col-md-2">
                            <Button onClick={this.getLyrics} bsStyle="info">Refresh for new Lyric </Button>
                        </div>
                    </div>
               
            );
    }
}

