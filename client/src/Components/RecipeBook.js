import axios from 'axios';
import React, { useEffect, useState } from 'react';

import { Grid, useMediaQuery } from '@material-ui/core';
import GitHub from '@material-ui/icons/GitHub';
import NavigateBefore from '@material-ui/icons/NavigateBefore';
import NavigateNext from '@material-ui/icons/NavigateNext';

import RecipePage from './RecipePage';

function previousPage(currentPage, setCurrentPage) {
  setCurrentPage(Math.max(0, currentPage - 1));
}

function getNewPages(recipePages, setRecipePages, numPages) {
  console.log('Getting new page');
  // axios.get(`/api/Recipes/Random/${3 * numPages}`)
  // axios.get(`https://localhost:44310/api/Recipes/Random/${3 * numPages}`)
  axios.get(`https://reciperoulette.azurewebsites.net/api/Recipes/Random/${3 * numPages}`)
    .then(response => {
      console.log(response);

      for (let i = 0; i < numPages; i++)
      {
        recipePages = recipePages.concat([response.data.slice(i * 3, (i + 1) * 3)]);
      }

      setRecipePages(recipePages);
    });
}

function nextPage(currentPage, setCurrentPage, recipePages, setRecipePages) {
  setCurrentPage(currentPage + 1);
  if (currentPage + 3 > recipePages.length) {
    getNewPages(recipePages, setRecipePages, 1);
  }
}

function RecipeBook(props) {
  const mdUp = useMediaQuery(theme => theme.breakpoints.up('md'));
  const [currentPage, setCurrentPage] = useState(0);
  const [recipePages, setRecipePages] = useState([]);

  useEffect(() => {
    console.log('mount');
    getNewPages(recipePages, setRecipePages, 2);
  }, []);

  return (
    <Grid container justify="center" style={{
      padding: 20,
      height: '90%'
    }}>
      <div style={{
        position: 'absolute',
        top: 0,
        left: 0,
        height: '100%',
        width: '50%'
      }}
      onClick={() => previousPage(currentPage, setCurrentPage)}>
        <NavigateBefore style={{
          position: 'absolute',
          bottom: mdUp ? '50%' : 20,
          left: 20,
          opacity: currentPage == 0 ? 0 : 1,
        }} />
      </div>
      <div style={{
        position: 'absolute',
        top: 0,
        right: 0,
        height: '100%',
        width: '50%'
      }}
      onClick={() => nextPage(currentPage, setCurrentPage, recipePages, setRecipePages)}>
        <NavigateNext style={{
          position: 'absolute',
          bottom: mdUp ? '50%' : 20,
          right: 20,
        }} />
      </div>
      <a style={{
        position: 'absolute',
        bottom: 20,
        width: 24,
        left: 'calc( 50% - 12px)',
        opacity: 0.5,
      }}
      href="https://github.com/ChineseElectricPanda/recipe-roulette" 
      target="_blank">
        <GitHub style={{width: 24}}/>
      </a>
      <Grid item xs={12} md={8} style={{
        height: '100%'
      }}>
        <div style={{
          display: 'grid',
          boxShadow: '0px 0px 30px 10px rgba(0, 0, 0, 0.15)',
          height: '100%'
        }}>
          {recipePages.map((recipePage, index) => {
            return <RecipePage
              key={index}
              pageNumber={index}
              recipes={recipePage}
              flipped={(currentPage > index)} />
          })}
        </div>
      </Grid>
    </Grid>);
}

export default RecipeBook;