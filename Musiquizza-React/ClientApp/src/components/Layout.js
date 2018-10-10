import React, { Component } from 'react';
import { Button, Col, Grid, Row } from 'react-bootstrap';
import { NavMenu } from './NavMenu';



class Layout extends Component {

    constructor(props,context) {
        super(props, context);
    
        this.authSpotify = this.authSpotify.bind(this);
    }

  displayName = Layout.name

  authSpotify(e) {
    e.preventDefault();
    window.location = '/api/Authentication'; 
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

export default Layout;