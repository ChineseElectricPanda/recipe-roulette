import React from 'react';

import { Divider, useMediaQuery } from '@material-ui/core';

import Recipe from './Recipe';

function RecipePage(props) {
  const xs = !useMediaQuery(theme => theme.breakpoints.up('sm'));
  return (
    <div className={"page-container"} style={{
      height: '100%',
      zIndex: 100000 - props.pageNumber,
      visibility: props.flipped ? 'collapse' : 'visible',
    }}>
      <div className={"page" + (props.flipped ? " flipped" : "")}>
        {props.recipes.slice(0, xs ? 1 : props.recipes.length).map((recipe, index, arr) => {
          return (
            <>
              <Recipe key={recipe.recipeID} recipe={recipe} />
              {index != arr.length - 1 ?
                <Divider variant="middle" /> : null
              }
            </>);
        })}
      </div>
    </div>);
}

export default RecipePage;