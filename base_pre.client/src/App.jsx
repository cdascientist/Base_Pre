import { useState, useEffect } from 'react';
import { createPortal } from 'react-dom';

const ProductDisplay = () => {
    const [allProducts, setAllProducts] = useState([]);
    const [currentIndex, setCurrentIndex] = useState(0);
    const [containers, setContainers] = useState({
        body: null,
        title: null
    });

    useEffect(() => {
        const bodyContainer = document.querySelector('.callout.right .body');
        const titleContainer = document.querySelector('.callout.right .title');

        if (bodyContainer && titleContainer) {
            setContainers({
                body: bodyContainer,
                title: titleContainer
            });
        }

        // Fetch data from API
        fetch('http://localhost:5000/api/ModelDbInits/Machine_Learning_Implementation_One/GetAllProducts')
            .then(response => response.json())
            .then(data => {
                setAllProducts(data);
            })
            .catch(error => console.error('Error fetching products:', error));
    }, []);

    const handlePrevious = () => {
        setCurrentIndex(prevIndex => (
            prevIndex > 0 ? prevIndex - 1 : allProducts.length - 1
        ));
    };

    const handleNext = () => {
        setCurrentIndex(prevIndex => (
            prevIndex < allProducts.length - 1 ? prevIndex + 1 : 0
        ));
    };

    if (!allProducts.length || !containers.body || !containers.title) {
        return null;
    }

    const currentProduct = allProducts[currentIndex];

    const buttonStyle = {
        color: '#57b3c0',
        padding: '8px 12px',
        margin: '2px 0',
        border: '1px solid rgba(87,179,192,0.2)',
        cursor: 'pointer',
        transition: 'all 0.3s ease',
        fontSize: '0.9em',
        letterSpacing: '1px',
        background: 'rgba(87,179,192,0.1)',
        textAlign: 'left',
        borderRadius: '4px',
        pointerEvents: 'auto'
    };

    const NavigationButtons = () => (
        <div className="flex items-center gap-2" style={{
            pointerEvents: 'auto',
            position: 'relative',
            zIndex: 1000,
            padding: '0 20px'
        }}>
            <button
                type="button"
                onClick={handlePrevious}
                style={{
                    ...buttonStyle,
                    ':hover': {
                        background: 'rgba(87,179,192,0.2)',
                        transform: 'translateX(5px)'
                    }
                }}
                className="hover:bg-[rgba(87,179,192,0.2)] hover:translate-x-1 active:bg-[rgba(87,179,192,0.3)] active:border-[rgba(87,179,192,0.4)] active:shadow-[0_0_15px_rgba(87,179,192,0.2)]"
            >
                Prev
            </button>
            <button
                type="button"
                onClick={handleNext}
                style={{
                    ...buttonStyle,
                    ':hover': {
                        background: 'rgba(87,179,192,0.2)',
                        transform: 'translateX(5px)'
                    }
                }}
                className="hover:bg-[rgba(87,179,192,0.2)] hover:translate-x-1 active:bg-[rgba(87,179,192,0.3)] active:border-[rgba(87,179,192,0.4)] active:shadow-[0_0_15px_rgba(87,179,192,0.2)]"
            >
                Next
            </button>
        </div>
    );

    const ProductFields = () => (
        <div style={{
            position: 'relative',
            padding: '0 10px',
            overflowY: 'auto',
            pointerEvents: 'auto',
            maxWidth: '200px',
            display: 'flex',
            flexDirection: 'column',
            gap: '30px'
        }}>
            {/* Prominent Product Name Display */}
            <div style={{
                padding: '12px',
                backgroundColor: 'rgba(87,179,192,0.1)',
                borderRadius: '4px',
                textAlign: 'center',
                color: '#57b3c0',
                fontSize: '1.2em',
                fontWeight: 'bold',
                border: '1px solid rgba(87,179,192,0.2)',
                marginTop: '-10px'
            }}>
                {currentProduct.productName}
            </div>

            {/* Other Product Details */}
            <ul className="list-none p-0 m-0 space-y-2">
                {[
                    { label: 'Source', value: currentProduct.source },
                    { label: 'ID', value: currentProduct.id },
                    { label: 'Type', value: currentProduct.productType },
                    { label: 'Price', value: `$${currentProduct.price.toFixed(2)}` },
                    { label: 'Quantity', value: currentProduct.quantity }
                ].map(({ label, value }) => (
                    <li key={label} style={{
                        padding: '8px',
                        margin: '5px 0',
                        borderBottom: '1px solid rgba(87,179,192,0.1)',
                        transition: 'all 0.3s ease',
                        color: '#a7d3d8',
                        fontSize: '0.9em',
                        whiteSpace: 'nowrap',
                        overflow: 'hidden',
                        textOverflow: 'ellipsis'
                    }}>
                        <strong>{label}:</strong> {value}
                    </li>
                ))}
            </ul>
        </div>
    );

    return (
        <>
            {createPortal(
                <div style={{ pointerEvents: 'auto', position: 'relative', zIndex: 1000 }}>
                    <NavigationButtons />
                </div>,
                containers.title
            )}
            {createPortal(<ProductFields />, containers.body)}
        </>
    );
};

export default ProductDisplay;