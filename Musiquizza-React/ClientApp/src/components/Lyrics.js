﻿import React, { Component } from 'react';
import { Button } from 'react-bootstrap';



export class Lyrics extends Component {
    constructor(props) {
        super(props);
        this.state = { lyrics: this.props.lyrics, spotifyUrl: `https://open.spotify.com/embed/track/${this.props.uri.replace("spotify:track:", "")}`};
        this.getLyrics = this.getLyrics.bind(this);
        
    }

    componentDidMount() {
       // this.getLyrics();
    }

     componentWillReceiveProps(nextProps){
        this.setState({lyrics: nextProps.lyrics, spotifyUrl: `https://open.spotify.com/embed/track/${this.props.uri.replace("spotify:track:", "")}`});
        
    }
    getLyrics() {
        this.props.getLyrics();
        this.setState({lyrics: this.props.lyrics, spotifyUrl: `https://open.spotify.com/embed/track/${this.props.uri.replace("spotify:track:", "")}`});
        }


    render() {

        return (
             
                    <div className="row">
                        <div className="col-md-8">
                            <b>{this.state.lyrics}</b>
                        </div>
                        <div className="col-md-2">
                        <iframe src={this.state.spotifyUrl} width="80" height="80" frameborder="0" allowtransparency="true" allow="encrypted-media"></iframe>
                        </div>
                        <div className="col-md-2">
                            <Button onClick={this.getLyrics} bsStyle="info">Refresh for new Lyric </Button>
                        </div>
                    </div>
               
            );
    }
}

