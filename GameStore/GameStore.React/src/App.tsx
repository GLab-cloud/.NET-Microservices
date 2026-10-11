import { Outlet } from 'react-router-dom';
import NavMenu from './components/NavMenu';

function App() {
    return (
        <div>
            <NavMenu />
            <div className="container">
                <Outlet />
            </div>
        </div>
    );
}

export default App;