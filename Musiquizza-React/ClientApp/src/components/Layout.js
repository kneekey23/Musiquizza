import React, { Component } from 'react';
import { Button, Col, Grid, Row } from 'react-bootstrap';
import { NavMenu } from './NavMenu';
import { API_ROOT } from './api-config';

export class Layout extends Component {

  displayName = Layout.name

  authSpotify() {
    fetch(`${API_ROOT}/Lyrics/AuthSpotify`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        }
    })
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
                            <Button bsStyle="warning" bsSize="large" className="playBtn" onClick={this.authSpotify}>Play the Game Now!</Button>
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
