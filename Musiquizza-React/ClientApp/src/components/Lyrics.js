import React, { Component } from 'react';
import { Button } from 'react-bootstrap';


export class Lyrics extends Component {
    constructor(props) {
        super(props);
        this.state = { lyrics: this.props.lyrics };
        this.getLyrics = this.getLyrics.bind(this);
    }

    getLyrics() {
        this.props.getLyrics();
        this.setState({lyrics: this.props.lyrics});
    }


    componentWillReceiveProps(nextProps){
        this.setState({lyrics: nextProps.lyrics});
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

