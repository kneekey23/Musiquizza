import React, { Component } from 'react';
import { BrowserRouter as Router, Route} from 'react-router-dom';
import Layout from './components/Layout';
import { Lyrics } from './components/Lyrics';
import { Quiz } from './components/Quiz';
import { API_ROOT } from './components/api-config';



var baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');

if (process.env.NODE_ENV === 'production') {
   
    baseUrl = "Prod";
}


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
        <Router basename={baseUrl}>
        <Layout>
            <section id="lyrics">
                <div className="container">
                      <Route path='/game' render={props => <Lyrics lyrics= {this.state.lyrics} getLyrics={this.getLyrics} {...props}/>} />
                      <Route path='/game' render={props => <Quiz uri={this.state.uri} {...props} />} />
                      <Route path='/admin' component={Quiz} />
                </div>
            </section>
        </Layout>
        </Router>
    );
  }
}
