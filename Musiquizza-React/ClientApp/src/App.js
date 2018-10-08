import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Lyrics } from './components/Lyrics';
import { Quiz } from './components/Quiz';
import { API_ROOT } from './components/api-config';

export default class App extends Component {

    constructor() {
        super();
        this.state = { lyrics: "", uri: "" };
        this.getLyrics = this.getLyrics.bind(this);
    }
  displayName = App.name

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
        .then(json => this.setState({ lyrics: json.lyrics, uri: json.uri }))
        .catch(error => console.log(error))
    }

  render() {
      return (
 
        <Layout>
            <section id="lyrics">
                <div className="container">
                    <Route path='/' render={props => <Lyrics lyrics= {this.state.lyrics} getLyrics={this.getLyrics} {...props}/>} />
                      <Route path='/' render={props => <Quiz uri={this.state.uri} {...props} />} />
                      <Route path='/admin' component={Quiz} />
                </div>
            </section>
        </Layout>
             
           
    );
  }
}
