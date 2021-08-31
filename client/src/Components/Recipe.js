import React from 'react';

import { Grid, useMediaQuery } from '@material-ui/core';
import ClockIcon from '@material-ui/icons/Schedule';
import StarIcon from '@material-ui/icons/Star';

import { decodeHtml } from '../CommonUtils';

function Recipe(props) {
  const recipe = props.recipe;
  const xs = useMediaQuery(theme => theme.breakpoints.down('xs'));

  return (
    <a href={recipe.recipeURL} target="_blank" style={{
      display: 'block',
      height: xs ? '100%' : 'calc( 33.333% + 8px )'
    }}>
      <Grid container spacing={2} style={{
        padding: 20,
        height: '100%'
      }}>
        <Grid item xs={12} sm={3} md={2} style={{
          height: xs ? '40%' : '100%'
        }}>
          <div style={{
            height: '100%',
            backgroundImage: 'url(' + recipe.imageURL + ')',
            backgroundSize: 'cover',
            backgroundPosition: 'center'
          }} />
        </Grid>
        <Grid item xs={12} sm={9} md={10} container style={{
          height: xs ? '60%' : 'default'
        }}>
          <Grid item>
            <div style={{
              marginBottom: 5,
              fontFamily: 'NothingYouCouldDo',
              fontSize: 30,
            }}>{decodeHtml(recipe.name)}</div>
            <div style={{
              marginBottom: 10
            }}>
              {recipe.recipeTags.map(recipeTag => {
                return (
                  <div key={recipeTag.tag.tagID} style={{
                    display: 'inline-block',
                    width: 35,
                    height: 35,
                    marginRight: 5,
                    borderRadius: '50%',
                    color: recipeTag.tag.color,
                    backgroundColor: recipeTag.tag.backgroundColor,
                    textAlign: 'center',
                  }}>
                    <span style={{
                      display: 'inline-block',
                      verticalAlign: 'middle',
                      lineHeight: '35px'
                    }}>
                      {recipeTag.tag.name}
                    </span>
                  </div>);
              })}
            </div>
            <div>
              {decodeHtml(recipe.description)}
            </div>
          </Grid>
          <Grid item style={{
            position: 'relative',
            width: '100%'
          }}>
            <span style={{
              position: 'absolute',
              bottom: 0
            }}>
              <ClockIcon style={{
                fontSize: 16,
                marginRight: 5,
                marginBottom: -2
              }} />
              <span style={{
                fontSize: 16
              }}>
                {recipe.totalTime}
              </span>
            </span>
            {recipe.rating ?
              <span style={{
                position: 'absolute',
                bottom: 0,
                right: 0
              }}>
                <StarIcon style={{
                  fontSize: 16,
                  marginRight: 5,
                  marginBottom: -2
                }} />
                <span style={{
                  fontSize: 16
                }}>
                  {recipe.rating.toFixed(2)}
                </span>
              </span> : null}
          </Grid>
        </Grid>
      </Grid>
    </a>);
}

export default Recipe;