import React, { Component } from 'react';
import { Row, Col, Button, FormControl, FormGroup, ControlLabel, Alert } from 'react-bootstrap';
import queryString from 'query-string';
import { API_ROOT } from './api-config';

export class Quiz extends Component {
    constructor(props) {
        super(props);
        this.state = { 
            artist: "", 
            title: "", 
            show: false, 
            displayError: false,
            playing: false,
            position: 0,
            duration: 0,
            token: "",
            deviceId: "",
            error: "",
            songUri:this.props.uri
         };
        this.handleDismiss = this.handleDismiss.bind(this);
        this.handleShow = this.handleShow.bind(this);
        this.handleTitleChange = this.handleTitleChange.bind(this);
        this.handleArtistChange = this.handleArtistChange.bind(this);
        this.handleFormReset = this.handleFormReset.bind(this);
        this.sendAnswers = this.sendAnswers.bind(this);
        this.getToken = this.getToken.bind(this);
        this.startPlayer = this.startPlayer.bind(this);
   

        this.playerCheckInterval = null;
    }

    componentWillReceiveProps(nextProps){
        this.setState({songUri: nextProps.uri});
       
    }

    componentDidMount() {
     
      //  this.setState({token: values.code}, () => this.startPlayer());
        this.getToken();
    }

    startPlayer(){
        if (this.state.token !== "") {
  
            this.playerCheckInterval = setInterval(() => this.checkForPlayer(), 1000);
        }
    }

    handleTitleChange(e) {
        this.setState({ title: e.target.value });
    }
    handleArtistChange(e) {
        this.setState({ artist: e.target.value });
    }

    handleShow() {
        this.setState({ show: true, displayError: false });
    }

    handleError() {
        this.setState({ show: true, displayError: true })
    }

    handleDismiss() {
        this.setState({ show: false });
    }

    handleFormReset() {
        this.setState({ artist: "", title: "" });
    }

    checkForPlayer() {
        const { token } = this.state;
      
        if (window.Spotify !== null) {
        clearInterval(this.playerCheckInterval);
          this.player = new window.Spotify.Player({
            name: "Nicki's Spotify Player",
            getOAuthToken: cb => { cb(token); }
          });
           this.createEventHandlers();
          
          // finally, connect!
          this.player.connect();
        }
      }

      createEventHandlers() {
        this.player.on('initialization_error', e => { console.error(e); });
        this.player.on('authentication_error', e => {
          console.error(e);
        });
        this.player.on('account_error', e => { console.error(e); });
        this.player.on('playback_error', e => { console.error(e); });
      
        // Playback status updates
        this.player.on('player_state_changed', state => { console.log(state); });
      
        // Ready
        this.player.on('ready', async data => {
          let { device_id } = data;
          console.log("Let the music play on!");
          await this.setState({ deviceId: device_id });
          this.transferPlaybackHere();
        });
      }

      transferPlaybackHere() {
        const { deviceId, token } = this.state;
        fetch("https://api.spotify.com/v1/me/player/", {
          method: "PUT",
          headers: {
            authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            "device_ids": [ deviceId ],
            "play": true,
            "uris": [this.state.songUri]
          }),
        });
      }

      getToken() {
              fetch(`${API_ROOT}/Authentication/token`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                }
              })
              .then(response => console.log(response));

      }


    sendAnswers(e) {
        e.preventDefault();
        this.handleDismiss();
        var url = `${API_ROOT}/Lyrics/`;
        

        fetch(url, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Artist: this.state.artist,
                Title: this.state.title,
                })
            })
            .then(response => response.json())
            .then((result) => {
               
                if (result.isCorrect) {
                    this.handleShow();
                }
                else {
                    this.handleError();
                }
                this.handleFormReset();
            },
            (error) => {
                this.handleShow();
            })
    }

    render() {
        let alert;
        if(this.state.show){
            alert = <ChooseAlert displayError={this.state.displayError} onDismiss={this.handleDismiss} />
        }
            return (
                <div>
                <Row id="quizForm">
                    <Col sm={12}>
                    {alert}
                        <form>
                            <FormGroup>
                                <ControlLabel>Artist</ControlLabel>
                                <FormControl
                                    type="text"
                                    value={this.state.artist}
                                    placeholder="Enter the artist"
                                    onChange={this.handleArtistChange}
                                />
                            </FormGroup>
                            <FormGroup>
                                <ControlLabel>Song Title</ControlLabel>
                                <FormControl
                                    type="text"
                                    value={this.state.title}
                                    placeholder="Enter the song title"
                                    onChange={this.handleTitleChange}
                                />
                            </FormGroup>
                            <Button bsStyle="primary" onClick={this.sendAnswers} > Submit</Button>
                        </form>
                    </Col>
                </Row>
                </div>
            )
        
    }
}

export class SuccessAlert extends Component {

    render() {
        return (<Alert bsStyle="success" onDismiss={this.props.onDismiss}>Success! You got the answer correctly! You are a musical genius!
        </Alert>);
    }
}

export class ErrorAlert extends Component {

    render() {
        return (<Alert bsStyle="danger" onDismiss={this.props.onDismiss}>Please try again. You are still musically challenged it seems.
                    </Alert>);
    }
}

export class ChooseAlert extends Component {
    isError = this.props.displayError;

    render() {
        if (this.isError === null) {
            return null
        }
        else if (!this.isError) {
            return <SuccessAlert onDismiss={this.props.onDismiss} />
        }
        else {
            return <ErrorAlert onDismiss={this.props.onDismiss} />
        }
    }
}