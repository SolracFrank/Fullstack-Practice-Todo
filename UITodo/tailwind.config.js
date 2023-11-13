/** @type {import('tailwindcss').Config} */
export default {
  content: [    "./index.html",
  "./src/**/*.{js,ts,jsx,tsx}",],
  theme: {
    extend: {
      colors:
      {
        'oscure':
        {
          100: '#FAF0E6' ,
          200 : '#B9B4C7',
          300 : '#5C5470',
          400 : '#352F44'
        },
        'calid':
        {
          100: '#FF5F9E' ,
          200 : '#E90064',
          300 : '#B3005E',
          350 : '#0B0083',
          400 : '#060047'
        }
        ,
        'day':
        {
          100 : '#F7FBFC',
          200 : '#D6E6F2',
          300 : '#B9D7EA',
          400 : '#769FCD',
        }
      },
      fontFamily:
      {
        raleway: "'Raleway','sans-serif'"
      }
    },
  },
  darkMode:"class",
  plugins: [],
}

