function retval = FirstDer(X,Y) 
%calculates the first derivatives at a set of points X with function values Y
        N = length(X);
        retval = zeros(N, 1);
        e = diff(X);
        h = e(1);
        for i=1:N
            if i == 1
            retval(i) = (-3*Y(i)+4*Y(i+1)-Y(i+2))/(2*h);
            elseif i == N
            retval (i) = (Y(i-2)-4*Y(i-1)+3*Y(i))/(2*h);
            else
            retval(i)=(-Y(i-1)+Y(i+1))/(2*h);
            end      
        end
end

