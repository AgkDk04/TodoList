import './App.css';
import Routers from './routes/routes'
import { configHeadersToken } from './utils/configHeadersToken'

function App() {
    const token = localStorage.getItem("token");
    if (token) {
        configHeadersToken(token);
    }

    return (
        <div className="App">
            <Routers />
        </div>
    );
}

export default App; 