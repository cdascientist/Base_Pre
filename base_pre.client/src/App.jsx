import { useState, useEffect } from 'react';

export default function App() {
    const [companies, setCompanies] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetch('http://localhost:5000/api/ModelDbInits/GetCustomers', {
            method: 'POST'
        })
            .then(response => response.json())
            .then(data => {
                console.log('Data received:', data);
                setCompanies(data);
                setLoading(false);
            })
            .catch(err => {
                console.error('Error:', err);
                setLoading(false);
            });
    }, []);

    if (loading) {
        return (
            <div style={{
                position: 'absolute',
                top: '50%',
                left: '50%',
                transform: 'translate(-50%, -50%)',
                color: '#57b3c0',
                fontSize: '24px',
                letterSpacing: '4px',
                textShadow: '0 0 10px rgba(87, 179, 192, 0.5)'
            }}>
                LOADING...
            </div>
        );
    }

    return (
        <div style={{
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%'
        }}>
            {/* Title */}
            <div style={{
                position: 'absolute',
                top: '5%',
                left: '50%',
                transform: 'translateX(-50%)',
                color: '#57b3c0',
                fontSize: '20px',
                letterSpacing: '4px',
                whiteSpace: 'nowrap',
                fontWeight: '400',
                textShadow: '0 0 10px rgba(87, 179, 192, 0.5)'
            }}>
                COMPANY DIRECTORY
            </div>

            {/* Records Container */}
            <div style={{
                position: 'absolute',
                top: '15%',
                left: '50%',
                transform: 'translateX(-50%)',
                width: '380px',
                display: 'flex',
                flexDirection: 'column',
                gap: '15px',
                maxHeight: '70%',
                overflowY: 'auto',
                padding: '20px 0'
            }}>
                {companies && companies.map((company, index) => (
                    <div
                        key={company.id || index}
                        style={{
                            padding: '20px',
                            border: '2px solid #57b3c0',
                            backgroundColor: 'rgba(87, 179, 192, 0.1)',
                            color: '#57b3c0',
                            textAlign: 'center',
                            cursor: 'pointer',
                            letterSpacing: '3px',
                            fontSize: '18px',
                            fontWeight: '500',
                            textTransform: 'uppercase',
                            textShadow: '0 0 10px rgba(87, 179, 192, 0.5)',
                            transition: 'all 0.3s ease'
                        }}
                        onMouseEnter={(e) => {
                            e.currentTarget.style.backgroundColor = 'rgba(87, 179, 192, 0.2)';
                            e.currentTarget.style.borderColor = '#d87930';
                            e.currentTarget.style.boxShadow = '0 0 20px rgba(87, 179, 192, 0.3)';
                            e.currentTarget.style.transform = 'translateX(10px)';
                        }}
                        onMouseLeave={(e) => {
                            e.currentTarget.style.backgroundColor = 'rgba(87, 179, 192, 0.1)';
                            e.currentTarget.style.borderColor = '#57b3c0';
                            e.currentTarget.style.boxShadow = 'none';
                            e.currentTarget.style.transform = 'translateX(0)';
                        }}
                    >
                        {company.companyName || 'UNNAMED'}
                    </div>
                ))}
            </div>
        </div>
    );
}