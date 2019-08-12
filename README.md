# Set Runner
Set Runner is a RESTful API that that provides aggregated setlist information for live sets that are publicly available on [1001Tracklists](1001Tracklists.com).

## Resource Endpoints 

* ``setrunner.net/api/v1``
    * ``/artists/{artistId}``
        * ``/tracks/popular``
        * ``/sets``
        * ``/sets/tracks``
    * ``sets/{setId}``
        *  ``/``
        * ``/tracks``
    * ``venues/{venueId}``
        * ``/tracks``

## Ids

* Artist/Venue IDs
    * Hyphen instead of space (e.g. 'svdden-death', 'edc-las-vegas')
    * Preserve punctuation such as '.' or '!' (e.g. 'dr.-fresch' or 'etc!etc!')
    * Replace ampersand (e.g. 'above-and-beyond', 'gabriel-and-dresden')
* Set IDs
    * Obtainable via querying or from the ``/artists/sets`` endpoint
* Track IDs
    * Obtainable via querying or from various ``*/tracks`` endpoints

## Filtering

* Most queries can be filtered by ``startDate`` and/or ``endDate`` in yyyy-MM-dd format

```html
// Gets the songs commonly played in Ben Nicky sets from the start of 2019 to now
www.setrunner.net/api/v1/artists/ben-nicky/sets/tracks?startDate=2019-01-01

// Gets the songs commonly played in Tomorrowland sets in 2019
www.setrunner.net/api/v1/venues/tomorrowland/tracks?startDate=2019-07-01&endDate=2019-07-31

// Gets Above & Beyond sets before 2019
www.setrunner.net/api/v1/artists/above-and-beyond/sets?endDate=2018-12-31
```

## Pagination

All endpoints support pagination via ``limit`` and ``skip`` parameters. 

```html
www.setrunner.net/api/v1/artists/tracks/popular?limit=10&skip=0
```

## Querying

Coming soon...