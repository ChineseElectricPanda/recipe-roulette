import { Box } from '@material-ui/core';
import { createTheme, ThemeProvider } from '@material-ui/core/styles';
import './App.css';

import RecipeBook from './Components/RecipeBook';

const theme = createTheme({});

function App() {
  return (
    <ThemeProvider theme={theme}>
      <div className="App" style={{
        height: '100%'
      }}>
        <Box display="flex" height="100%" justifyContent="center" alignItems="center">
          <RecipeBook />
        </Box>
      </div>
    </ThemeProvider>
  );
}

export default App;
