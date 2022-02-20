//imports
var express = require('express');
var session = require('express-session')
var axios = require('axios');

//parameters
var port = 3000;
var sessionAge = 86400000;
var apiHost = 'http://localhost:30083';
var mapApyKey = 'AIzaSyBO_UIvXJaTQ0XTPt9Vy5rDbmkTs5qTkPU';

//settings
var app = express();
app.use(express.urlencoded(({ extended: true })));
app.use(session({ secret: 'keyboard cat', resave: true, saveUninitialized: true, cookie: { maxAge: sessionAge } }));
app.set('view engine', 'ejs');
app.use(express.static('public'));

//login page
app.get('/', (req, res) => {
  res.render('login', {
    error: req.query.error
  });
});

//login action
app.post('/login', (req, res) => {
  axios.post(apiHost + '/authorizations', {
    email: req.body.email,
    password: req.body.password
  })
    .then(function (apiRes) {
      req.session.token = apiRes.data.token;
      req.session.userId = apiRes.data.userId;

      res.redirect('story-list');
    })
    .catch(function (apiErr) {
      res.redirect('/?error=' + apiErr.response.status + ' (' + apiErr.response.statusText + ')');
    });
});

//list of stories
app.get('/story-list', async (req, res) => {
  var textFiler = req.query.text ? req.query.text : "";

  try {
    var storiesApiRes = await axios.get(apiHost + '/stories/?text=' + textFiler, {
      headers: {
        'Authorization': 'Bearer ' + req.session.token,
        'SortType': 'DateDesc',
        'PageIndex': '1',
        'PageSize': '30'
      }
    });

    var userApiRes = await axios.get(apiHost + '/users/' + req.session.userId, {
      headers: {
        'Authorization': 'Bearer ' + req.session.token
      }
    });

    res.render('story-list', {
      user: userApiRes.data,
      stories: storiesApiRes.data
    });
  } catch (apiErr) {
    res.send(apiErr.response.status + ' (' + apiErr.response.statusText + ')');
  }
});

//story detail
app.get('/story-detail/:id', async (req, res) => {
  try {
    var storyApiRes = await axios.get(apiHost + '/stories/' + req.params.id, {
      headers: {
        'Authorization': 'Bearer ' + req.session.token,
      }
    });

    var userApiRes = await axios.get(apiHost + '/users/' + storyApiRes.data.userId, {
      headers: {
        'Authorization': 'Bearer ' + req.session.token
      }
    });

    res.render('story-detail', {
      story: storyApiRes.data,
      user: userApiRes.data,
      mapApyKey: mapApyKey
    });
  }
  catch (apiErr) {
    res.send(apiErr.response.status + ' (' + apiErr.response.statusText + ')');
  }
});

//new story
app.get('/new-story', (req, res) => {
  res.render('new-story', {
    error: req.query.error,
    mapApyKey: mapApyKey
  });
});

//publish story action
app.post('/publish-story', (req, res) => {
  axios.post(apiHost + '/stories', {
    type: req.body.type == "" ? null : req.body.type,
    title: req.body.title,
    tale: req.body.tale,
    latitude: req.body.latitude,
    longitude: req.body.longitude,
    userId: req.session.userId
  },
    {
      headers: {
        'Authorization': 'Bearer ' + req.session.token
      }
    })
    .then(function (apiRes) {
      res.redirect('story-list');
    })
    .catch(function (apiErr) {
      res.redirect('new-story/?error=' + apiErr.response.status + ' (' + apiErr.response.statusText + ')');
    });
});

//point of interest
app.get('/poi', (req, res) => {
  res.render('poi', {
    mapApyKey: mapApyKey
  });
});

//logout action
app.get('/logout', (req, res) => {
  req.session.token = null;
  req.session.userId = null;

  res.redirect('/');
});

//startup
app.listen(port, () => {
  console.log(`http://localhost:${port}`);
});