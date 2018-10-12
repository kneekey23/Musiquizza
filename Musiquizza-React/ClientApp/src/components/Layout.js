import React, { Component } from 'react';
import { Button, Col, Grid, Row } from 'react-bootstrap';
import { NavMenu } from './NavMenu';
import { API_ROOT } from './api-config';



class Layout extends Component {

    constructor(props,context) {
        super(props, context);
        this.state = {
            token: ''
        };
        this.authSpotify = this.authSpotify.bind(this);
        this.getToken = this.getToken.bind(this);
    }

  displayName = Layout.name

  authSpotify(e) {
    e.preventDefault();
    window.location = `${API_ROOT}/Authentication`; 
    }

    getToken(val) {
        console.log(val);
        this.setState({token: val});
    }

    render() {

    return (
      <Grid fluid>
          <Row>
                <NavMenu />
                    <header className="masthead">
                        <div className="container">
                            <div className="intro-text">
                                <div className="intro-lead-in">Welcome To Musiquizza!</div>
                            <div className="intro-heading text-uppercase">For the Musically challenged</div>
                            <Button bsStyle="warning" bsSize="large" className="playBtn" href="#lyrics">Play the Game</Button>
                            </div>
                        </div>
                    </header>
            </Row>
            <Row>
                <Col sm={12}>
                    {this.props.children}
                </Col>
            </Row>
      </Grid>
    );
  }
}

export default Layout;