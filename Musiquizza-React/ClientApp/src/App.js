import React, { Component } from 'react';
import { BrowserRouter as Router, Route} from 'react-router-dom';
import Layout from './components/Layout';
import { Lyrics } from './components/Lyrics';
import { Quiz } from './components/Quiz';



var baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');

if (process.env.NODE_ENV === 'production') {
   
    baseUrl = "Prod";
}


export default class App extends Component {

  displayName = App.name

  render() {
      return (
        <Router basename={baseUrl}>
        <Layout>
            <section id="lyrics">
                <div className="container">
                      <Route path='/game' component={Lyrics}/>
                      <Route path='/game' component={Quiz} />
                      <Route path='/admin' component={Quiz} />
                </div>
            </section>
        </Layout>
        </Router>
    );
  }
}
